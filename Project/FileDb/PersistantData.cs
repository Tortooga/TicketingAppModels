namespace Models
{
    class PersistantData{
        public static readonly char[] AllowedCharsArray = 
        {
            // Uppercase letters
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
            
            // Lowercase letters
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
            
            // Numbers
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
            
            // Underscore
            '_', ' '
        };

        public static readonly string[] AllowedTypesArray = 
        {
            "String", "Int32", "Boolean", "Single"
        };

        public static readonly char[] IntAllowedChars = {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
        };

        public static readonly char[] FloatAllowedChars = {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '.'
        };
    }
}