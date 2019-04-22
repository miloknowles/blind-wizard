using System.Collections;
using System.Collections.Generic;

// All of the primitive concepts in our game such as 'Region' and 'Element'
// are placed into their own namespace to avoid colliding with other Unity
// or C# things with the same name.
namespace Primitives {

/*
 * Defines all of the regions accessible in the game.
 */
public enum Region {
    Ocean,
    Volcano,
    Underground,
    Cloud,
    Plains,
    MMTTG
};

/*
 * Defines the four element types that the player and enemy can take on.
 * We explicitly assign integer values to each of the enum types so that
 * they have a "super effective" ordering Air > Earth > Water > Fire > Air.
 *
 * Note: Reordering these will break ElementOrder.Compare()!
 */
public enum Element {
    Fire,
    Water,
    Earth,
    Air
};

/*
 * Defines the four visual attributes that types can have.
 */
public enum Attribute {
    Scaly,
    Furry,
    Smooth
};

/*
 * Provides a helper method for comparing two elements.
 */
public class ElementOrdering {
    /*
     * Returns +1 if el1 is effective against el2
     * Returns 0 if el1 is neutral against el2
     * Returns -1 if el1 is ineffective against el2
     */
    public static int Compare(Element el1, Element el2)
    {
        int enum_val1 = (int)el1;
        int enum_val2 = (int)el2;

        // In case the number of elements changes later, calculate it here.
        int num_elements = System.Enum.GetNames(typeof(Element)).Length;
        
        // Case 1: if el2 is AFTER el1, then el1 is ineffective.
        if (enum_val2 == (enum_val1 + 1) % num_elements) {
            return -1;
        
        // Case 2: if el1 is AFTER el2, then el1 is effective.
        } else if (enum_val1 == (enum_val2 + 1) % num_elements) {
            return 1;

        // Case 3: everything else is neutral.
        } else {
            return 0;
        }
    }
};

} // namespace Primitives