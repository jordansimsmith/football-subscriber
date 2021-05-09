import { Button, ButtonGroup } from '@chakra-ui/button';
import { ArrowLeftIcon, ArrowRightIcon, RepeatIcon } from '@chakra-ui/icons';
import { Center } from '@chakra-ui/layout';
import { Spinner } from '@chakra-ui/spinner';
import {
  Table,
  TableCaption,
  Tbody,
  Tfoot,
  Th,
  Thead,
  Tr,
  Td,
} from '@chakra-ui/table';
import React from 'react';
import { useQuery } from 'react-query';
import { IFixture } from '../types/types';
import { FixtureRow } from './FixtureRow';

interface FixturesTableProps {
  competitionId?: number;
  fromDate: Date;
  toDate: Date;
  onFromDateChange: (date: Date) => void;
  onToDateChange: (date: Date) => void;
}

export const FixturesTable: React.FC<FixturesTableProps> = ({
  competitionId,
  fromDate,
  toDate,
  onFromDateChange,
  onToDateChange,
}) => {
  const { data, isLoading } = useQuery<IFixture[]>(
    ['fixtures', competitionId, fromDate.toDateString(), toDate.toDateString()],
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

  const handleDateChange = (dateChange: number) => () => {
    const newFromDate = new Date(fromDate);
    const newToDate = new Date(toDate);

    newFromDate.setDate(newFromDate.getDate() + dateChange);
    newToDate.setDate(newToDate.getDate() + dateChange);

    onFromDateChange(newFromDate);
    onToDateChange(newToDate);
  };

  const handlePreviousRound = handleDateChange(-7);
  const handleNextRound = handleDateChange(7);

  const handleResetRound = () => {
    const newFromDate = new Date();
    const newToDate = new Date();

    newToDate.setDate(newToDate.getDate() + 7);

    onFromDateChange(newFromDate);
    onToDateChange(newToDate);
  };

  if (isLoading) {
    return (
      <Center>
        <Spinner />
      </Center>
    );
  }

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

      <Tfoot>
        <Tr>
          <Td colSpan={4}>
            <Center>
              <ButtonGroup>
                <Button
                  disabled={!competitionId}
                  leftIcon={<ArrowLeftIcon />}
                  onClick={handlePreviousRound}
                >
                  Previous Round
                </Button>
                <Button
                  leftIcon={<RepeatIcon />}
                  onClick={handleResetRound}
                  disabled={!competitionId}
                >
                  Reset
                </Button>
                <Button
                  disabled={!competitionId}
                  rightIcon={<ArrowRightIcon />}
                  onClick={handleNextRound}
                >
                  Next Round
                </Button>
              </ButtonGroup>
            </Center>
          </Td>
        </Tr>
      </Tfoot>
    </Table>
  );
};
