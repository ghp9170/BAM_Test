import { AstronautDuty } from './AstronautDuty';
import { Person } from './Person';

export interface GetAstronautDutiesByNameResult {
  success: boolean;
  message: string;
  responseCode: number;
  person: Person;
  astronautDuties: AstronautDuty[];
}