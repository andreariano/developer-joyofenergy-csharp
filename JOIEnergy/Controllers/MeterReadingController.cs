﻿using System;
using System.Collections.Generic;
using System.Linq;
using JOIEnergy.Domain;
using JOIEnergy.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JOIEnergy.Controllers
{
    [Route("readings")]
    public class MeterReadingController : Controller
    {
        private readonly IMeterReadingService _meterReadingService;

        public MeterReadingController(IMeterReadingService meterReadingService)
        {
            _meterReadingService = meterReadingService;
        }
        // POST api/values
        [HttpPost("store")]
        public ObjectResult Post([FromBody] MeterReadings meterReadings)
        {
            // FIXME: Having this sort of validation at the controller level breaks the SRP as the controller should only be 
            // dealing with HTTP parsing, and also part of the domain logic is out of the "business layer", breaking cohesion
            if (!IsMeterReadingsValid(meterReadings))
            {
                return new BadRequestObjectResult("Internal Server Error");
            }
            _meterReadingService.StoreReadings(meterReadings.SmartMeterId, meterReadings.ElectricityReadings);
            return new OkObjectResult("{}");
        }

        private static bool IsMeterReadingsValid(MeterReadings meterReadings)
        {
            String smartMeterId = meterReadings.SmartMeterId;
            List<ElectricityReading> electricityReadings = meterReadings.ElectricityReadings;
            return smartMeterId != null && smartMeterId.Any()
                    && electricityReadings != null && electricityReadings.Any();
        }

        [HttpGet("read/{smartMeterId}")]
        public ObjectResult GetReading(string smartMeterId)
        {
            return new OkObjectResult(_meterReadingService.GetReadings(smartMeterId));
        }
    }
}
