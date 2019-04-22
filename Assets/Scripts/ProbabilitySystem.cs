using System.Collections;
using System.Collections.Generic;
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

    //========================== DISTRIBUTION DEFINITIONS ==============================
    // For each region, stores the probability of an enemy being each element.
    private Dictionary<Region, ElementDistribution> P_ELEMENT_GIVEN_REGION = new Dictionary<Region, ElementDistribution>() {
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
    private Dictionary<Element, AttributeDistribution> P_ATTR_GIVEN_ELEMENT = new Dictionary<Element, AttributeDistribution> {
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
    public Region SampleRegionUniform()
    {
        int num_regions = System.Enum.GetNames(typeof(Region)).Length;
        return (Region)(UnityEngine.Random.Range(0, num_regions));
    }

    /*
     * Add noise to probability distributions
     */ 
    public void RandomizeRegionDistributions()
    {
        foreach (Region region in P_ELEMENT_GIVEN_REGION.Keys)
        {
            List<double> noise;
			var random = new Random();

			if (region != Primitives.Region.MMTTG)
			{
                noise = new List<double> { 0.1, 0.1, -0.1, -0.1 };
                foreach (Element elt in P_ELEMENT_GIVEN_REGION[region].Keys)
				{
					int index = random.Next(noise.Count);
                    P_ELEMENT_GIVEN_REGION[region][elt] += noise[index];
					noise.RemoveAt(index);
				}
			}
			else
			{
				noise = new List<double> { -0.1, 0.1, 0 };
                foreach (Element elt in P_ELEMENT_GIVEN_REGION[region].Keys)
				{
					int index = random.Next(noise.Count);
					if (elt != Primitives.Element.Water)
					{
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
    public Element SampleElementGivenRegion(Region region)
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
    public Attribute SampleAttributeGivenElement(Element element)
    {
        AttributeDistribution p_attr_given_element = P_ATTR_GIVEN_ELEMENT[element];
        List<Attribute> attr_list = new List<Attribute>(p_attr_given_element.Keys);
        List<double> p_attr_given_element_list = new List<double>(p_attr_given_element.Values);
        return attr_list[SampleIndex(p_attr_given_element_list)];
    }

    //========================= PRIVATE HELPER METHODS ==============================
    /*
     * Generic method for sampling from a discrete distribution, represented
     * as an array of doubles. The values should add up to the 1.0, but this
     * method will normalize just in case.
     *
     * Returns a sampled INDEX from distribution.
     */
    private int SampleIndex(List<double> distribution)
    {
        System.Random r = new System.Random();
        double random_val = r.NextDouble();

        double sum = 0.0;
        for (int i = 0; i < distribution.Count; ++i) { sum += distribution[i]; }

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
};