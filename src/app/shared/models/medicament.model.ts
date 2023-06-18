import { Pharmacy } from "./pharmacy.model";
export class Medicament {
    Id: number;
    Name: string;
    Description: string;
    Count: number;
    Pharmacy: Pharmacy | null;
    PharmacyId: number;

    constructor(Id: number, Name: string, Description: string,
        Count: number, Pharmacy: Pharmacy | null, PharmacyId: number,)
    {
        this.Id = Id;
        this.Name = Name;
        this.Description = Description;
        this.Count = Count;
        this.Pharmacy = Pharmacy;
        this.PharmacyId = PharmacyId;
    }
}