import { Spinner } from '@chakra-ui/spinner';
import { Table, TableCaption, Tbody, Th, Thead, Tr } from '@chakra-ui/table';
import React from 'react';
import { useQuery } from 'react-query';
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
  const { data, isLoading } = useQuery(
    ['fixtures', competitionId, fromDate, toDate],
    async () => {
      if (!competitionId) {
        return [];
      }

      const url = new URL('http://localhost:5000/fixtures');
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
    { initialData: [] },
  );

  if (isLoading) {
    return <Spinner />;
  }

  const fixtureRows = React.useMemo(() => {
    return data.map((f) => <FixtureRow key={f.id} fixture={f} />);
  }, [data]);

  return (
    <Table variant="simple">
      <TableCaption>
        Fixtures between {fromDate.toDateString()} - {toDate.toDateString()}
      </TableCaption>

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
