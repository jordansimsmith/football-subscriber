import { Center } from '@chakra-ui/layout';
import { Table, Tbody, Td, Th, Thead, Tr } from '@chakra-ui/table';
import React from 'react';
import { IFixture } from '../types/types';
import { FixtureRow } from './FixtureRow';

interface FixturesTableProps {
  fixtures: IFixture[];
}

export const FixturesTable: React.FC<FixturesTableProps> = ({ fixtures }) => {
  const fixtureRows = React.useMemo(() => {
    if (!fixtures?.length) {
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

    return fixtures.map((f) => <FixtureRow key={f.id} fixture={f} />);
  }, [fixtures]);

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
