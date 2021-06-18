import { useMediaQuery, Box } from '@chakra-ui/react';
import React from 'react';
import { useQuery } from 'react-query';
import { IFixture } from '../types/types';
import { FixtureControls } from './FixtureControls';
import { FixturesList } from './FixturesList';
import { FixturesTable } from './FixturesTable';

interface FixtureDisplayProps {
  competitionId?: number;
  fromDate: Date;
  toDate: Date;
  onFromDateChange: (date: Date) => void;
  onToDateChange: (date: Date) => void;
}

export const FixtureDisplay: React.FC<FixtureDisplayProps> = ({
  competitionId,
  fromDate,
  toDate,
  onFromDateChange,
  onToDateChange,
}) => {
  const [isLargeScreen] = useMediaQuery('(min-width: 800px)');

  const { data } = useQuery<IFixture[]>(
    ['fixtures', competitionId, fromDate.toDateString(), toDate.toDateString()],
    async () => {
      if (!competitionId) {
        return [];
      }

      const url = new URL(`${process.env.NEXT_PUBLIC_SERVER_BASE}/fixtures`);
      const params = {
        competitionId: competitionId.toString(),
        fromDate: fromDate.toISOString(),
        toDate: toDate.toISOString(),
      };
      url.search = new URLSearchParams(params).toString();

      const res = await fetch(url.toString());
      const data = await res.json();
      return data;
    },
  );

  return (
    <Box
      border="1px"
      borderColor="gray.200"
      padding="20px"
      borderRadius="md"
      marginY="20px"
      overflowX="auto"
      background="white"
    >
      {isLargeScreen ? (
        <FixturesTable fixtures={data} />
      ) : (
        <FixturesList fixtures={data} />
      )}

      <FixtureControls
        disabled={!competitionId}
        fromDate={fromDate}
        toDate={toDate}
        onFromDateChange={onFromDateChange}
        onToDateChange={onToDateChange}
      />
    </Box>
  );
};

export default FixtureDisplay;
