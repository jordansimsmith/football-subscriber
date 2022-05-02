import { UserProvider } from '@auth0/nextjs-auth0';
import { ChakraProvider, Flex } from '@chakra-ui/react';
import { AppProps } from 'next/app';
import { QueryClient, QueryClientProvider } from 'react-query';
import { HeaderBar } from '../components/HeaderBar';
import '../styles/globals.css';

const queryClient = new QueryClient();

const MyApp = ({ Component, pageProps }: AppProps): JSX.Element => {
  return (
    <QueryClientProvider client={queryClient}>
      <UserProvider>
        <ChakraProvider>
          <Flex direction="column">
            <HeaderBar />
            <Component {...pageProps} />
          </Flex>
        </ChakraProvider>
      </UserProvider>
    </QueryClientProvider>
  );
};

export default MyApp;
