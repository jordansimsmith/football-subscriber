import React from 'react';
import { NextPage } from 'next';
import Head from 'next/head';
import { CompetitionSelect } from '../components/CompetitionSelect';
import { FixturesTable } from '../components/FixturesTable';
import { Box, Container, Divider, Heading } from '@chakra-ui/layout';

const Index: NextPage = () => {
  const [competitionId, setCompetitionId] = React.useState<number>(null);
  const [fromDate, setFromDate] = React.useState<Date>(new Date());
  const [toDate, setToDate] = React.useState<Date>(() => {
    const date = new Date();
    date.setDate(date.getDate() + 7);
    return date;
  });

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
            Competition
          </Heading>
          <Divider marginY="15px" />
          <CompetitionSelect
            value={competitionId}
            onChange={setCompetitionId}
          />
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
            competitionId={competitionId}
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
