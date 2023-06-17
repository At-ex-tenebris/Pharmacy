import { Country } from './country.model';

export class Region {
  id: number;
  login: string;
  password: string;
  name: string;
  country: Country | null;
  countryId: number;
  constructor(
    id: number,
    login: string,
    password: string,
    name: string,
    countryId: number,
    country: Country | null
  ) {
    this.id = id;
    this.login = login;
    this.password = password;
    this.name = name;
    this.country = country;
    this.countryId = countryId;
  }
}
