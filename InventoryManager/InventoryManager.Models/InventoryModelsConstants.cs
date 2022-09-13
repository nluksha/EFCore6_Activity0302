using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManager.Models
{
    public class InventoryModelsConstants
    {
        public const int MAX_DESCRIPTION_LENGTH = 250;
        public const int MAX_NAME_LENGTH = 100;
        public const int MAX_NOTES_LENGTH = 2000;
        public const int MIN_NOTES_LENGTH = 10;
        public const int MAX_USERID_LENGTH = 50;

        public const int MINIMUM_QAUNTITY = 0;
        public const int MAXIMUM_QAUNTITY = 1000;
        public const double MINIMUM_PRICE = 0.0;
        public const double MAXIMUM_PRICE = 25_000.0;

        public const int MAX_COLORLAVUE_LENGTH = 25;
        public const int MAX_COLORNAME_LENGTH = 25;

        public const int MAX_PLAYERNAME_LENGTH = 50;
        public const int MAX_PLAYERDESCRIPTION_LENGTH = 500;

        public const int MAX_GENRENAME_LENGTH = 50;

        public const int MAX_STOCKSYMBOL_LENGTH = 10;
        public const int MAX_COMPANYNAME_LENGTH = 150;
        public const int MAX_CITY_LENGTH = 50;
        public const int MAX_FIRSTNAME_LENGTH = 50;
        public const int MAX_LASTNAME_LENGTH = 50;
    }
}
