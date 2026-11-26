using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLD_DataAccessLayer;

namespace DVLD_BusinessLayer
{
    public class clsCountry
    {
        public int ID { get; }
        public string CountryName { get; }


        private clsCountry(int ID, string CountryName)

        {
            this.ID = ID;
            this.CountryName = CountryName;


          
        }

        public static DataTable GetAllCountries()
        {
            return CountriesData.GetAllCountries();

        }

        public static clsCountry Find(int ID)
        {

            string CountryName = "";
           

            int CountryID = -1;

            if (CountriesData.GetCountryInfoByID(ID, ref CountryName))

                return new clsCountry(ID, CountryName);
            else
                return null;

        }

        public static clsCountry Find(string CountryName)
        {

            int ID = -1;


            if (CountriesData.GetCountryInfoByName(CountryName, ref ID))

                return new clsCountry(ID, CountryName);
            else
                return null;

        }
    }
}
