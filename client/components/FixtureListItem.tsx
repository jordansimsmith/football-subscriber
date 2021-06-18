import { Divider, HStack, ListItem, Text, VStack } from '@chakra-ui/react';
import Image from 'next/image';
import React from 'react';
import { IFixture } from '../types/types';

interface FixtureListItemProps {
  fixture: IFixture;
}

export const FixtureListItem: React.FC<FixtureListItemProps> = ({
  fixture,
}) => {
  return (
    <ListItem>
      <VStack padding="20px">
        <HStack>
          <Text fontWeight="bold" marginRight="5px">
            {fixture.homeTeamName}
          </Text>
          <Image
            src={fixture.homeOrganisationLogo?.replace('//', 'https://')}
            alt="Home team"
            width="40px"
            height="40px"
            objectFit="contain"
          />
        </HStack>

        <Text color="gray.600">vs</Text>

        <HStack>
          <Text fontWeight="bold" marginRight="5px">
            {fixture.awayTeamName}
          </Text>
          <Image
            src={fixture.awayOrganisationLogo?.replace('//', 'https://')}
            alt="Away team"
            width="40px"
            height="40px"
            objectFit="contain"
          />
        </HStack>

        <Text>{new Date(fixture.date).toLocaleString()}</Text>

        <Text>{fixture.venueName}</Text>
        <Text color="gray.600">{fixture.address}</Text>
      </VStack>
      <Divider />
    </ListItem>
  );
};
