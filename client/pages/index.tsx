import { useUser } from '@auth0/nextjs-auth0';
import { Alert, AlertIcon } from '@chakra-ui/alert';
import { ExternalLinkIcon } from '@chakra-ui/icons';
import {
  Box,
  Center,
  Container,
  Divider,
  Heading,
  Link,
  Text,
} from '@chakra-ui/layout';
import { NextPage } from 'next';
import Head from 'next/head';
import React from 'react';
import { CompetitionSelect } from '../components/CompetitionSelect';
import { IOption } from '../types/types';
import dynamic from 'next/dynamic';

// uses media queries, can't be rendered on the server
const FixtureDisplay = dynamic(() => import('../components/FixtureDisplay'), {
  ssr: false,
  loading: () => null,
});

const Index: NextPage = () => {
  const [competition, setCompetition] = React.useState<IOption>();
  const [fromDate, setFromDate] = React.useState<Date>(new Date());
  const [toDate, setToDate] = React.useState<Date>(() => {
    const date = new Date();
    date.setDate(date.getDate() + 7);
    return date;
  });

  const { user, isLoading } = useUser();

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
        {!user && !isLoading && (
          <Alert status="info" marginTop="20px">
            <AlertIcon />
            Log in to be notified when your team&apos;s fixtures change.
          </Alert>
        )}
        <Box
          border="1px"
          borderColor="gray.200"
          padding="20px"
          borderRadius="md"
          marginY="20px"
          background="white"
        >
          <Heading as="h3" size="md">
            Competition
          </Heading>
          <Divider marginY="15px" />
          <CompetitionSelect value={competition} onChange={setCompetition} />
        </Box>

        <FixtureDisplay
          competitionId={competition?.value}
          toDate={toDate}
          fromDate={fromDate}
          onToDateChange={setToDate}
          onFromDateChange={setFromDate}
        />

        <Center color="gray.600">
          <Text>Jordan Sim-Smith 2021</Text>
          <Text mx="4px">·</Text>
          <Link
            href="https://github.com/jordansimsmith/football-subscriber"
            isExternal
          >
            View the source code <ExternalLinkIcon mx="2px" />
          </Link>
        </Center>
      </Container>
    </Box>
  );
};

export default Index;
