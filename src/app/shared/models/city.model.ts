import { Region } from "./region.model";
export class City{
  id: number;
  login: string;
  password: string;
  cityName: string;
  region: Region | null;
  regionId: number;
  constructor(Id: number, login: string, password: string, cityName: string, regionId: number, region: Region | null) {
    this.id = Id;
    this.login = login;
    this.password = password;
    this.cityName = cityName;
    this.regionId = regionId;
    this.region = region;
  }
}