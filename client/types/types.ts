export interface ICompetition {
  id: number;
  name: string;
}

export interface IFixture {
  address: string;
  awayOrganisationId: string;
  awayOrganisationLogo: string;
  awayTeamId: number;
  awayTeamName: string;
  competitionId: number;
  date: string;
  homeOrganisationId: number;
  homeOrganisationLogo: string;
  homeTeamId: number;
  homeTeamName: string;
  id: number;
  venueId: number;
  venueName: string;
}

export interface ITeam {
  name: string;
}

export interface ISubscription {
  id: number;
  userId: string;
  teamId: number;
  teamName: string;
}

export interface IOption {
  label: string;
  value: number;
}
