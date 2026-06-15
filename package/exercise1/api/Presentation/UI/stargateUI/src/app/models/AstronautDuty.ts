export interface AstronautDuty {
  id: number;
  astronautId: number;
  rank: string;
  dutyTitle: string;
  dutyStartDate: string;
  dutyEndDate?: string | null;
  isCurrent: boolean;
}