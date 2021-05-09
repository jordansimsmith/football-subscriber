import { Box, Container, Flex, Heading, Text } from '@chakra-ui/layout';
import React from 'react';

export const HeaderBar: React.FC<{}> = () => {
  return (
    <Box backgroundColor="teal.100" padding="10px">
      <Container maxW="container.xl">
        <Flex justifyContent="space-between">
          <Heading>Football Subscriber</Heading>

          <Flex alignItems="center">
            <Text>User</Text>
            <Text paddingLeft="10px">Logout</Text>
          </Flex>
        </Flex>
      </Container>
    </Box>
  );
};
