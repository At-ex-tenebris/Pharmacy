namespace pharmacyApi.Models {
    public class Medicament {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Count { get; set; }

        public Pharmacy? Pharmacy { get; set; }
        public int PharmacyId { get; set; }
    }
}
