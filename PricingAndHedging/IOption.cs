﻿using System;

namespace PricingAndHedging.FinalExam
{
    public interface IOption
    {
        double Price { get; }

        double Delta { get; }
    }
}
