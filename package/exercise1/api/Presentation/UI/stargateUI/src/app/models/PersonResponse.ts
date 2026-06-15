import { Person } from "./Person";

export interface PersonResponse {
  people: Person[];
  success: boolean;
  message: string;
  responseCode: number;
}
