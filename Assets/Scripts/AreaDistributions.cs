using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaDistributions : MonoBehaviour
{
    public enum Area { Ocean, Volcano, Underground, Cloud, Plains, MMTTG };
    public Area currArea;

    public enum Element { Fire, Water, Earth, Air }

    private List<KeyValuePair<Element, double>> currDist;

    // these are the prior distributions over types (as written in the brainstorming doc on the google drive

    List<KeyValuePair<Element, double>> oceanDist = {new KeyValuePair<Element, double>(Element.Fire,0.1),
                                                     new KeyValuePair<Element, double>(Element.Air, 0.2),
                                                     new KeyValuePair<Element, double>(Element.Earth,0.3),
                                                     new KeyValuePair<Element, double>(Element.Water,0.4)};

    List<KeyValuePair<Element, double>> volcanoDist = {new KeyValuePair<Element, double>(Element.Water,0.1),
                                                       new KeyValuePair<Element, double>(Element.Air, 0.2),
                                                       new KeyValuePair<Element, double>(Element.Earth,0.3),
                                                       new KeyValuePair<Element, double>(Element.Fire,0.4)};

    List<KeyValuePair<Element, double>> undergroundDist = {new KeyValuePair<Element, double>(Element.Air,0.1),
                                                           new KeyValuePair<Element, double>(Element.Water, 0.2),
                                                           new KeyValuePair<Element, double>(Element.Fire,0.3),
                                                           new KeyValuePair<Element, double>(Element.Earth,0.4)};

    List<KeyValuePair<Element, double>> cloudDist = {new KeyValuePair<Element, double>(Element.Earth,0.1),
                                                     new KeyValuePair<Element, double>(Element.Fire, 0.2),
                                                     new KeyValuePair<Element, double>(Element.Element.Water,0.3),
                                                     new KeyValuePair<Element, double>(Element.Air,0.4)};

    List<KeyValuePair<Element, double>> plainsDist = {new KeyValuePair<Element, double>(Element.Fire,0.25),
                                                     new KeyValuePair<Element, double>(Element.Air, 0.25),
                                                     new KeyValuePair<Element, double>(Element.Earth,0.25),
                                                     new KeyValuePair<Element, double>(Element.Water,0.25)};

    List<KeyValuePair<Element, double>> mmttgDist = {new KeyValuePair<Element, double>(Element.Water,0),
                                                     new KeyValuePair<Element, double>(Element.Earth, 0.1),
                                                     new KeyValuePair<Element, double>(Element.Air,0.1),
                                                     new KeyValuePair<Element, double>(Element.Fire,0.8)};

    // randomly select the current area to battle in
    public Area randSelectArea()
    {
        currArea = (Area)(UnityEngine.Random.Range(0, Enum.GetNames(typeof(Area)).Length));

        switch (currArea)
        {
            case Area.Ocean:
                currDist = oceanDist;
            case Area.Volcano:
                currentDist = volcanoDist;
            case Area.Underground:
                currDist = undergroundDist;
            case Area.Cloud:
                currDist = cloudDist;
            case Area.Plains:
                currDist = plainsDist;
            case Area.MMTTG:
                currDist = mmttgDist;
        }

        return currArea;
    }

    // generate an enemy type given the current area
    public Element randGenerateEnemyType()
    {
        Random r = new Random();
        double diceRoll = r.NextDouble();

        double cumulative = 0.0;
        for (int i = 0; i < currDist.Count; i += 1)
        {
            cumulative += currDist[i].Value;
            if (diceRoll < cumulative)
            {
                return currDist[i].Key;
            }
        }

    }

}
