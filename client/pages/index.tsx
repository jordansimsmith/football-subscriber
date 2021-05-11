import React from 'react';
import { NextPage } from 'next';
import Head from 'next/head';
import { CompetitionSelect } from '../components/CompetitionSelect';
import { FixturesTable } from '../components/FixturesTable';
import { Box, Container, Divider, Heading } from '@chakra-ui/layout';
import { IOption } from '../types/types';
import { Alert, AlertIcon } from '@chakra-ui/alert';
import { useUser } from '@auth0/nextjs-auth0';

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
            Log in to be notified when your team's fixtures change.
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
          <FixturesTable
            competitionId={competition?.value}
            fromDate={fromDate}
            toDate={toDate}
            onFromDateChange={setFromDate}
            onToDateChange={setToDate}
          />
        </Box>
      </Container>
    </Box>
  );
};

export default Index;
