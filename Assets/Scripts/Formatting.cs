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
        * Formats a float to a string.
        * 
        * If the value is less than 1000, it will return the value as a string.
        * If the value is more than 1000, it will return the value divided by 1000 and rounded to one decimal.
        *
        * TODO:
        * - Add support for millions, billions, etc.
        */
    public static string FloatToShortString(float value = 0, int decimals = 0) {
        if (value < 1000) {
            return ((int)value).ToString();
        } else {
            decimal rounded = System.Math.Round((decimal)(value / 1000), decimals);
            return rounded.ToString() + "K";
        }
    }

}

