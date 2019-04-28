using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Primitives; // Namespace for Element, Region, Attribute.

/*
 * Create more readable names for the entries in each probability distribution.
 * Note: these must go at the top of the file or C# throws errors.
 */
using ElementDistribution = System.Collections.Generic.Dictionary<Primitives.Element, double>;
using AttributeDistribution = System.Collections.Generic.Dictionary<Primitives.Attribute, double>;

/*
 * Provides utils for probabilistically generating events and sampling from
 * distributions. Internally, distributions are stored as dictionaries to
 * explicitly map from enum types to corresponding probability values.
 */
public static class ProbabilitySystem {
    private static System.Random rng = new System.Random();

    // Since the ProbabilitySystem doesn't derive from MonoBehaviour (static classes cant)
    // we need a way to call Debug.Log and see messages in the unity editor.
    public class Debugger : MonoBehaviour {
        public void Log(string line) { Debug.Log(line); }
    };

    //========================== DISTRIBUTION DEFINITIONS ==============================
    // For each region, stores the probability of an enemy being each element.
    private static Dictionary<Region, ElementDistribution> P_ELEMENT_GIVEN_REGION = new Dictionary<Region, ElementDistribution>() {
        {Region.Ocean, new ElementDistribution() {
            {Element.Fire, 0.1}, {Element.Air, 0.2}, {Element.Earth, 0.3}, {Element.Water, 0.4} } },
        {Region.Volcano, new ElementDistribution() {
            {Element.Fire, 0.4}, {Element.Air, 0.2}, {Element.Earth, 0.3}, {Element.Water, 0.1} } },
        {Region.Underground, new ElementDistribution() {
            {Element.Fire, 0.3}, {Element.Air, 0.1}, {Element.Earth, 0.4}, {Element.Water, 0.2} } },
        {Region.Cloud, new ElementDistribution() {
            {Element.Fire, 0.2}, {Element.Air, 0.4}, {Element.Earth, 0.1}, {Element.Water, 0.3} } },
        {Region.Plains, new ElementDistribution() {
            {Element.Fire, 0.25}, {Element.Air, 0.25}, {Element.Earth, 0.25}, {Element.Water, 0.25} } },
        {Region.MMTTG, new ElementDistribution() {
            {Element.Fire, 0.8}, {Element.Air, 0.1}, {Element.Earth, 0.1}, {Element.Water, 0.0} } }
    };

    // For each enemy element, stores the probability of taking on a visual attribute.
    private static Dictionary<Element, AttributeDistribution> P_ATTR_GIVEN_ELEMENT = new Dictionary<Element, AttributeDistribution> {
        {Element.Water, new AttributeDistribution() {
            {Attribute.Scaly, 0.4}, {Attribute.Furry, 0.3}, {Attribute.Smooth, 0.3} } },
        {Element.Fire, new AttributeDistribution() {
            {Attribute.Scaly, 0.4}, {Attribute.Furry, 0.1}, {Attribute.Smooth, 0.5} } },
        {Element.Air, new AttributeDistribution() {
            {Attribute.Scaly, 0.3}, {Attribute.Furry, 0.4}, {Attribute.Smooth, 0.3} } },
        {Element.Earth, new AttributeDistribution() {
            {Attribute.Scaly, 0.1}, {Attribute.Furry, 0.5}, {Attribute.Smooth, 0.4} } }
    };


    //============================ PUBLIC API ===========================================
    /*
     * Sample a random region from a uniform distribution.
     */
    public static Region SampleRegionUniform()
    {
        int num_regions = System.Enum.GetNames(typeof(Region)).Length;
        return (Region)(UnityEngine.Random.Range(0, num_regions));
    }

    /*
     * Add random noise to the probability distributions. Although the probability distribution
     * (i.e elements given region) are fixed at the start of the game, we want each playthrough 
     * of the game to be unique. This method modifies the distributions defined at the top of this
     * class by adding small deviations to the base distribution.
     */ 
    public static void RandomizeRegionDistributions()
    {
        foreach (Region region in P_ELEMENT_GIVEN_REGION.Keys) {
            List<double> noise;
			var random = new System.Random();

			if (region != Primitives.Region.MMTTG) {
                noise = new List<double> { 0.1, 0.1, -0.1, -0.1 };
                foreach (Element elt in P_ELEMENT_GIVEN_REGION[region].Keys) {
					int index = random.Next(noise.Count);
                    P_ELEMENT_GIVEN_REGION[region][elt] += noise[index];
					noise.RemoveAt(index);
				}
			} else {
				noise = new List<double> { -0.1, 0.1, 0 };
                foreach (Element elt in P_ELEMENT_GIVEN_REGION[region].Keys) {
					int index = random.Next(noise.Count);
					if (elt != Primitives.Element.Water) {
                        P_ELEMENT_GIVEN_REGION[region][elt] += noise[index];
						noise.RemoveAt(index);
					}
				}
			}
        }
    }

    /*
     * Sample an enemy element given a region.
     */
    public static Element SampleElementGivenRegion(Region region)
    {
        ElementDistribution p_element_given_region = P_ELEMENT_GIVEN_REGION[region];
        
        // Get aligned lists of elements and their associated probabilities.
        // https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2.keys?view=netframework-4.8
        List<Element> element_list = new List<Element>(p_element_given_region.Keys);
        List<double> p_element_given_region_list = new List<double>(p_element_given_region.Values);
        return element_list[SampleIndex(p_element_given_region_list)];
    }

    /*
     * Sample an attribute for a given enemy element.
     */
    public static Attribute SampleAttributeGivenElement(Element element)
    {
        AttributeDistribution p_attr_given_element = P_ATTR_GIVEN_ELEMENT[element];
        List<Attribute> attr_list = new List<Attribute>(p_attr_given_element.Keys);
        List<double> p_attr_given_element_list = new List<double>(p_attr_given_element.Values);
        return attr_list[SampleIndex(p_attr_given_element_list)];
    }

    /*
     * Perform Bayesian updates to calculate the true probability distribution
     * over enemy elements given the region, attributes, and actions.
     */
    public static ElementDistribution ComputePosterior(
            Region region,
            Attribute attr,
            List<PlayerActionResult> player_actions,
            List<EnemyActionResult> enemy_actions)
    {
        // Step 1: Incorporate the prior information from the region.
        ElementDistribution prior = P_ELEMENT_GIVEN_REGION[region];
        ElementDistribution posterior = Normalize(prior);

        // Step 2: Incorporate the attribute that was observed.
        ElementDistribution p_attr_all_elements = new ElementDistribution(); // i.e p(furry | water)
        List<Element> element_list = new List<Element>(P_ATTR_GIVEN_ELEMENT.Keys);
        for (int i = 0; i < element_list.Count; ++i) {
            Element element = element_list[i];
            p_attr_all_elements.Add(element, P_ATTR_GIVEN_ELEMENT[element][attr]);
        }
        posterior = Normalize(Multiply(posterior, p_attr_all_elements));

        // Step 3: Incorporate player actions.
        for (int i = 0; i < player_actions.Count; ++i) {
            PlayerActionResult action = player_actions[i];
            
            for (int el_i = 0; el_i < element_list.Count; ++i) {
                Element possible_enemy_element = element_list[i];
                int matchup_multiplier = ElementOrdering.Compare(action.element, possible_enemy_element);
                double accuracy_vs_element = ClampZeroOne(action.attack.accuracy + matchup_multiplier * Constants.SUPER_EFFECTIVE_ACCURACY_BONUS);

                // For this possible enemy element, what is the probability of the outcome observed?
                double p_of_result = action.successful ? accuracy_vs_element : (1.0 - accuracy_vs_element);
                posterior[possible_enemy_element] *= p_of_result;
            }
        }
        posterior = Normalize(posterior); // Normalize again.

        // Step 4: Incorporate enemy actions.
        for (int i = 0; i < enemy_actions.Count; ++i) {
            EnemyActionResult action = enemy_actions[i];

            for (int el_i = 0; el_i < element_list.Count; ++i) {
                Element possible_enemy_element = element_list[i];
                int matchup_multiplier = ElementOrdering.Compare(possible_enemy_element, action.player_element_during);
                double accuracy_vs_player = ClampZeroOne(action.accuracy + matchup_multiplier * Constants.SUPER_EFFECTIVE_ACCURACY_BONUS);

                // For this possible enemy element, what is the probability that their attack hit/missed?
                double p_of_result = action.successful ? accuracy_vs_player : (1.0 - accuracy_vs_player);
                posterior[possible_enemy_element] *= p_of_result;
            }
        }
        posterior = Normalize(posterior); // Normalize again.

        return posterior;
    }

    //========================= PRIVATE HELPER METHODS ==============================
    /*
     * Elementwise-multiply two ElementDistributions (dictionary mapping a type to a probility).
     */
    private static ElementDistribution Multiply(ElementDistribution d1, ElementDistribution d2)
    {
        ElementDistribution multiplied = new ElementDistribution();
        List<Element> elements_list = new List<Element>(d1.Keys);

        for (int i = 0; i < elements_list.Count; ++i) {
            Element el = elements_list[i];
            System.Diagnostics.Debug.Assert(d2.ContainsKey(el)); // Make sure that d2 has all of the same keys as d1.
            multiplied.Add(el, d1[el] * d2[el]);
        }

        return multiplied;
    }

    /*
     * Normalize an ElementDistribution.
     */
    private static ElementDistribution Normalize(ElementDistribution d)
    {
        ElementDistribution normalized = new ElementDistribution();
        List<Element> elements_list = new List<Element>(d.Keys);

        double sum = 0.0;
        for (int i = 0; i < elements_list.Count; ++i) { sum += d[elements_list[i]]; }
        for (int i = 0; i < elements_list.Count; ++i) {
            normalized.Add(elements_list[i], d[elements_list[i]] / sum);
        }

        return normalized;
    }

    /*
     * Generic method for sampling from a discrete distribution, represented
     * as an array of doubles. The values should add up to the 1.0, but this
     * method will normalize just in case.
     *
     * Returns a sampled INDEX from distribution.
     */
    private static int SampleIndex(List<double> distribution)
    {
        double random_val = rng.NextDouble();

        double sum = 0.0;
        for (int i = 0; i < distribution.Count; ++i) { sum += distribution[i]; }

        // debug.Log("Sum: " + sum.ToString());

        double cumul = 0.0;
        for (int i = 0; i < distribution.Count; ++i) {
            double p_normalized = distribution[i] / sum;

            cumul += p_normalized;

            if (random_val <= cumul) {
                return i;
            }
        }

        // Should not reach here; cumulative distribution should have returned a sample by now.
        return 0; // Satisfy build rules (all code paths must return).
    }

    // Clamp a number to a valid range for probabilities.
    public static double ClampZeroOne(double value)
    {
        return (value < 0.0) ? 0.0 : (value > 1.0) ? 1.0 : value;
    }
};