import React from 'react';
import { useRouter } from 'next/router';
import { useUser } from '@auth0/nextjs-auth0';
import { Button, ButtonGroup } from '@chakra-ui/button';
import { Box, Container, Flex, Heading } from '@chakra-ui/layout';

export const HeaderBar: React.FC<{}> = () => {
  const { user } = useUser();
  const router = useRouter();

  const handleLogin = () => router.push('/api/auth/login');
  const handleLogout = () => router.push('/api/auth/logout');
  const handleSubscriptions = () => router.push('/subscriptions');

  return (
    <Box backgroundColor="teal.100" padding="10px">
      <Container maxW="container.xl">
        <Flex justifyContent="space-between">
          <Heading>Football Subscriber</Heading>

          <Flex alignItems="center">
            <ButtonGroup>
              {user && (
                <Button
                  variant="ghost"
                  colorScheme="teal"
                  onClick={handleSubscriptions}
                >
                  Subscriptions
                </Button>
              )}
              <Button
                variant="ghost"
                colorScheme="teal"
                onClick={user ? handleLogout : handleLogin}
              >
                {user ? 'Logout' : 'Log in'}
              </Button>
            </ButtonGroup>
          </Flex>
        </Flex>
      </Container>
    </Box>
  );
};
