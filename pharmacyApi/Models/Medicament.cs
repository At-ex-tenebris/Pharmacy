namespace pharmacyApi.Models {
    public class Medicament {
        public int Id { get; set; }             // Поле идентификатора
        public string Name { get; set; }        // Поле наименования
        public string Description { get; set; } // Поле описания
        public int Count { get; set; }          // Поле количества

        public Pharmacy Pharmacy { get; set; }  // Поле связи с аптекой
        public int PharmacyId { get; set; }
    }
}
