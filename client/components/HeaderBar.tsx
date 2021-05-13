import React from 'react';
import NextLink from 'next/link';
import { useRouter } from 'next/router';
import { useUser } from '@auth0/nextjs-auth0';
import { Button } from '@chakra-ui/button';
import {
  Box,
  Container,
  Heading,
  Wrap,
  WrapItem,
  Link,
} from '@chakra-ui/layout';

export const HeaderBar: React.FC<{}> = () => {
  const { user } = useUser();
  const router = useRouter();

  const handleLogin = () => router.push('/api/auth/login');
  const handleLogout = () => router.push('/api/auth/logout');
  const handleSubscriptions = () => router.push('/subscriptions');

  return (
    <Box backgroundColor="teal.100" padding="10px">
      <Container maxW="container.xl">
        <Wrap justify="space-between" spacing="0">
          <WrapItem>
            <Link as={NextLink} href="/">
              <Heading cursor="pointer">Football Subscriber</Heading>
            </Link>
          </WrapItem>

          <WrapItem>
            {user && (
              <WrapItem>
                <Button
                  variant="ghost"
                  colorScheme="teal"
                  onClick={handleSubscriptions}
                >
                  Subscriptions
                </Button>
              </WrapItem>
            )}
            <WrapItem>
              <Button
                variant="ghost"
                colorScheme="teal"
                onClick={user ? handleLogout : handleLogin}
              >
                {user ? 'Logout' : 'Log in'}
              </Button>
            </WrapItem>
          </WrapItem>
        </Wrap>
      </Container>
    </Box>
  );
};
