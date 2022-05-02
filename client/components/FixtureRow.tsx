import React from 'react';
import Image from 'next/image';
import { Flex, Tooltip, Wrap, WrapItem } from '@chakra-ui/react';
import { Tr, Td } from '@chakra-ui/table';
import { IFixture } from '../types/types';

interface FixtureRowProps {
  fixture: IFixture;
}

export const FixtureRow = ({ fixture }: FixtureRowProps): JSX.Element => {
  return (
    <Tr>
      <Td>
        <Flex>
          <Wrap>
            <WrapItem>
              <Image
                src={fixture.homeOrganisationLogo?.replace('//', 'https://')}
                alt="Home team"
                width="40px"
                height="40px"
                objectFit="contain"
              />
            </WrapItem>
            <WrapItem alignItems="center">{fixture.homeTeamName}</WrapItem>
          </Wrap>
        </Flex>
      </Td>
      <Td>
        <Flex>
          <Wrap>
            <WrapItem>
              <Image
                src={fixture.awayOrganisationLogo?.replace('//', 'https://')}
                alt="Away team"
                width="40px"
                height="40px"
                objectFit="contain"
              />
            </WrapItem>
            <WrapItem alignItems="center">{fixture.awayTeamName}</WrapItem>
          </Wrap>
        </Flex>
      </Td>
      <Td>{new Date(fixture.date).toLocaleString()}</Td>
      <Td>
        <Tooltip label={fixture.address}>{fixture.venueName}</Tooltip>
      </Td>
    </Tr>
  );
};
