import { Flex, Image, Tooltip, Wrap, WrapItem } from '@chakra-ui/react';
import { Tr, Td } from '@chakra-ui/table';
import React from 'react';

interface FixtureRowProps {
  fixture: any;
}

export const FixtureRow: React.FC<FixtureRowProps> = ({ fixture }) => {
  return (
    <Tr>
      <Td>
        <Flex>
          <Wrap>
            <WrapItem>
              <Image
                src={fixture.homeOrganisationLogo}
                alt="Home team logo"
                boxSize="40px"
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
                src={fixture.awayOrganisationLogo}
                alt="Away team logo"
                boxSize="40px"
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
