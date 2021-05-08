import { NextPage } from 'next';
import Head from 'next/head';
import { CompetitionSelect } from '../components/CompetitionSelect';

const Index: NextPage = () => {
  return (
    <div>
      <Head>
        <title>Football Subscriber</title>
        <meta
          name="description"
          content="Caching and Notification layer in front of the http://www.auckland.org.nz football fixtures API"
        />
        <link rel="icon" href="/favicon.ico" />
      </Head>

      <main>
        <h1>Football Subscriber</h1>

        <CompetitionSelect />
      </main>

      <footer>Jordan Sim-Smith</footer>
    </div>
  );
};

export default Index;
