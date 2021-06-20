using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebApiMedMyStem
{
    //класс найденного лекарства
    public class FindMedical
    {
        public medicament medicament { get; set; } // найденное лекарство
        public bool Commercial { get; set; } // фалаг, что найденное лекарство было найдено по торговому названию
    }
}
