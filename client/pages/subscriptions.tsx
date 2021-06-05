import { withPageAuthRequired } from '@auth0/nextjs-auth0';
import { Box, Container, Divider, Heading } from '@chakra-ui/layout';
import { NextPage } from 'next';
import Head from 'next/head';
import { SubscriptionForm } from '../components/SubscriptionForm';
import { SubscriptionsList } from '../components/SubscriptionsList';

const SubscriptionsPage: NextPage<{}> = () => {
  return (
    <Box height="full" bg="gray.50">
      <Head>
        <title>Football Subscriber</title>
        <meta
          name="description"
          content="Caching and Notification layer in front of the http://www.auckland.org.nz football fixtures API"
        />
        <link rel="icon" href="/favicon.ico" />
      </Head>

      <Container maxW="container.xl">
        <Box
          border="1px"
          borderColor="gray.200"
          padding="20px"
          borderRadius="md"
          marginY="20px"
          background="white"
        >
          <Heading as="h3" size="md">
            New Subscription
          </Heading>
          <Divider marginY="15px" />
          <SubscriptionForm />
        </Box>

        <Box
          border="1px"
          borderColor="gray.200"
          padding="20px"
          borderRadius="md"
          marginY="20px"
          background="white"
        >
          <Heading as="h3" size="md">
            Subscriptions
          </Heading>
          <Divider marginY="15px" />
          <SubscriptionsList />
        </Box>
      </Container>
    </Box>
  );
};

export const getServerSideProps = withPageAuthRequired();

export default SubscriptionsPage;
