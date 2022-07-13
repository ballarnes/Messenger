﻿using System;

namespace Hospital.BusinessLogic.Models.Requests
{
    public class AddAppointmentRequest
    {
        public int DoctorId { get; set; }

        public int OfficeId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string PatientName { get; set; } = null!;
    }
}
