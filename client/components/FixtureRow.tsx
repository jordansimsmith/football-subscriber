import { Image, Wrap, WrapItem } from '@chakra-ui/react';
import { Tr, Td } from '@chakra-ui/table';
import React from 'react';

interface FixtureRowProps {
  fixture: any;
}

export const FixtureRow: React.FC<FixtureRowProps> = ({ fixture }) => {
  return (
    <Tr>
      <Td>
        <Wrap>
          <WrapItem>
            <Image
              src={fixture.homeOrganisationLogo}
              alt="Home team logo"
              boxSize="40px"
              objectFit="contain"
            />
          </WrapItem>
          <WrapItem>{fixture.homeTeamName}</WrapItem>
        </Wrap>
      </Td>
      <Td>
        <Wrap>
          <WrapItem>
            <Image
              src={fixture.awayOrganisationLogo}
              alt="Away team logo"
              boxSize="40px"
              objectFit="contain"
            />
          </WrapItem>
          <WrapItem>{fixture.awayTeamName}</WrapItem>
        </Wrap>
      </Td>
      <Td>{new Date(fixture.date).toLocaleString()}</Td>
      <Td>{fixture.venueName}</Td>
    </Tr>
  );
};
