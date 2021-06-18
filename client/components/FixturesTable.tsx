import { Center } from '@chakra-ui/layout';
import { Spinner } from '@chakra-ui/spinner';
import { Table, Tbody, Td, Th, Thead, Tr } from '@chakra-ui/table';
import React from 'react';
import { useQuery } from 'react-query';
import { IFixture } from '../types/types';
import { FixtureRow } from './FixtureRow';

interface FixturesTableProps {
  competitionId?: number;
  fromDate: Date;
  toDate: Date;
}

export const FixturesTable: React.FC<FixturesTableProps> = ({
  competitionId,
  fromDate,
  toDate,
}) => {
  const { data, isLoading } = useQuery<IFixture[]>(
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

  const fixtureRows = React.useMemo(() => {
    if (!data?.length) {
      return (
        <Tr>
          <Td colSpan={4}>
            <Center>
              No fixtures available for the current competition and round.
            </Center>
          </Td>
        </Tr>
      );
    }

    return data.map((f) => <FixtureRow key={f.id} fixture={f} />);
  }, [data]);

  if (isLoading) {
    return (
      <Center>
        <Spinner />
      </Center>
    );
  }

  return (
    <Table variant="simple">
      <Thead>
        <Tr>
          <Th>Home Team</Th>
          <Th>Away Team</Th>
          <Th>Time</Th>
          <Th>Venue</Th>
        </Tr>
      </Thead>

      <Tbody>{fixtureRows}</Tbody>
    </Table>
  );
};
