using System;
using System.Collections.Generic;
using System.Linq;
using JOIEnergy.Enums;

namespace JOIEnergy.Domain
{
    public class PricePlan
    {
        public Supplier EnergySupplier { get; set; }
        public decimal UnitRate { get; set; }
        public IList<PeakTimeMultiplier> PeakTimeMultiplier { get; set; }

        // FIXME: This solution is not following a DDD pattern but rather a "business layer" pattern, so I'd say we could move this to 
        // PricePlanService.cs and make this model anemic
        public decimal GetPrice(DateTime datetime)
        {
            var multiplier = PeakTimeMultiplier.FirstOrDefault(m => m.DayOfWeek == datetime.DayOfWeek);

            if (multiplier?.Multiplier != null)
            {
                return multiplier.Multiplier * UnitRate;
            }
            else
            {
                return UnitRate;
            }
        }
    }

    public class PeakTimeMultiplier
    {
        public DayOfWeek DayOfWeek { get; set; }
        public decimal Multiplier { get; set; }
    }
}
