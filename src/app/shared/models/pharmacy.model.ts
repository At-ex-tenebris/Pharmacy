import { City } from "./city.model";
export class Pharmacy{
  id: number;
  login: string;
  password: string;
  pharmacyName: string;
  latitude: number;
  longitude: number;
  city: City | null;
  cityId: number;
  
  constructor(Id: number, login: string, password: string, pharmacyName: string, latitude: number, longitude: number, cityId: number, city: City | null) {
    this.id = Id;
    this.login = login;
    this.password = password;
    this.pharmacyName = pharmacyName;
    this.latitude = latitude;
    this.longitude = longitude;
    this.cityId = cityId;
    this.city = city;
  }
}