import { Divider, List, Text } from '@chakra-ui/react';
import React from 'react';
import { IFixture } from '../types/types';
import { FixtureListItem } from './FixtureListItem';

interface FixturesListProps {
  fixtures: IFixture[];
}

export const FixturesList: React.FC<FixturesListProps> = ({ fixtures }) => {
  if (!fixtures?.length) {
    return (
      <>
        <Text>
          No fixtures available for the current competition and round.
        </Text>
        <Divider marginTop="10px" />
      </>
    );
  }

  return (
    <List>
      {fixtures.map((f) => (
        <FixtureListItem key={f.id} fixture={f} />
      ))}
    </List>
  );
};
