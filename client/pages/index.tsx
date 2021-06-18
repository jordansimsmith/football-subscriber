import React from 'react';
import { NextPage } from 'next';
import Head from 'next/head';
import { CompetitionSelect } from '../components/CompetitionSelect';
import { FixturesTable } from '../components/FixturesTable';
import {
  Box,
  Center,
  Container,
  Divider,
  Heading,
  Link,
  Text,
} from '@chakra-ui/layout';
import { IFixture, IOption } from '../types/types';
import { Alert, AlertIcon } from '@chakra-ui/alert';
import { useUser } from '@auth0/nextjs-auth0';
import { ExternalLinkIcon } from '@chakra-ui/icons';
import { useMediaQuery } from '@chakra-ui/react';
import { FixturesList } from '../components/FixturesList';
import { FixtureControls } from '../components/FixtureControls';
import { useQuery } from 'react-query';

const Index: NextPage = () => {
  const [competition, setCompetition] = React.useState<IOption>();
  const [fromDate, setFromDate] = React.useState<Date>(new Date());
  const [toDate, setToDate] = React.useState<Date>(() => {
    const date = new Date();
    date.setDate(date.getDate() + 7);
    return date;
  });

  const { user, isLoading } = useUser();

  const [isLargeScreen] = useMediaQuery('(min-width: 800px)');

  const { data } = useQuery<IFixture[]>(
    [
      'fixtures',
      competition?.value,
      fromDate.toDateString(),
      toDate.toDateString(),
    ],
    async () => {
      if (!competition?.value) {
        return [];
      }

      const url = new URL(`${process.env.NEXT_PUBLIC_SERVER_BASE}/fixtures`);
      const params = {
        competitionId: competition.value.toString(),
        fromDate: fromDate.toISOString(),
        toDate: toDate.toISOString(),
      };
      url.search = new URLSearchParams(params).toString();

      const res = await fetch(url.toString());
      const data = await res.json();
      return data;
    },
  );

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

        <Box
          border="1px"
          borderColor="gray.200"
          padding="20px"
          borderRadius="md"
          marginY="20px"
          overflowX="auto"
          background="white"
        >
          {isLargeScreen ? (
            <FixturesTable fixtures={data} />
          ) : (
            <FixturesList fixtures={data} />
          )}

          <FixtureControls
            disabled={!competition?.value}
            fromDate={fromDate}
            toDate={toDate}
            onFromDateChange={setFromDate}
            onToDateChange={setToDate}
          />
        </Box>
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
