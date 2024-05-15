/**
    * Formatting Class.
    *
    * This class is used to format the text in the game.
    * It's a collection of helper-functions made to make the code more readable,
    * and the formatting more consistent.
    *
    * Author(s): William Fridh
    */

public static class Formatting {

    /**
        * Formats a float to a string with prefix.
        */
    public static string FloatToShortString(float value = 0, int decimals = 0) {
        if (value >= 1000000000) {
            decimal rounded = System.Math.Round((decimal)(value / 1000000000), decimals);
            return rounded.ToString() + "B";
        } else
        if (value >= 1000000) {
            decimal rounded = System.Math.Round((decimal)(value / 1000000), decimals);
            return rounded.ToString() + "M";
        } else if (value >= 1000) {
            decimal rounded = System.Math.Round((decimal)(value / 1000), decimals);
            return rounded.ToString() + "K";
        } else {
            return ((int)value).ToString();
        }
    }

}

