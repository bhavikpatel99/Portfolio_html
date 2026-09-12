export interface Country {
  id: number;
  code: string;
  name: string;
}

export interface StateItem {
  id: number;
  countryId: number;
  name: string;
}
