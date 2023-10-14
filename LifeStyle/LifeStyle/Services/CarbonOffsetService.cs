using GoogleApi.Entities.Translate.Common.Enums;

namespace LifeStyle.Services
{
    public static class CarbonOffsetService
    {


        //double EmissionsPerKilometer;

        //// Define vehicle-specific values
        //double fuelEfficiencyKilometersPerLiter = 12.0; // Example: 12 kilometers per liter for a car
        //double emissionsPerLiterOfGasoline = 2.3;       // Example: 2.3 kilograms of CO2 per liter of gasoline

        //// Calculate emissions per kilometer
        //EmissionsPerKilometer = emissionsPerLiterOfGasoline / fuelEfficiencyKilometersPerLiter;


        // Constants for emissions factors (in CO2 equivalent per unit)
        private const double EmissionsPerKilometer = 0.2; // Example: Emissions per kilometer traveled
        private const double EmissionsPerKilogram = 2.3;  // Example: Emissions per kilogram of waste generated

        public static double CalculateOffsetForTravel(double milesTraveled, int peoples, string mode)
        {
            //// Calculate emissions for travel
            //double emissions = kilometersTraveled * EmissionsPerKilometer;

            //// Assume some offsetting activity that reduces emissions
            //double offset = 0.1 * emissions; // For example, planting trees or renewable energy

            if(mode == "bike")
                return milesTraveled * 33;
            if(mode == "walk")
                return milesTraveled * 0;

            double offset = 0;
            switch (peoples)
            {
                case 0:
                    offset = milesTraveled * 400;
                    break;
                case 1:
                    offset = milesTraveled * 264;
                    break;
                case 2:
                    offset = milesTraveled * 123; 
                    break;
            }
            return offset;
        }

        public static double CalculateOffsetForWaste(double kilogramsWasteGenerated)
        {
            // Calculate emissions for waste generation
            double emissions = kilogramsWasteGenerated * EmissionsPerKilogram;

            // Assume some offsetting activity that reduces emissions
            double offset = 0.2 * emissions; // For example, recycling or waste-to-energy projects

            return offset;
        }
    }
}

