import { ChakraProvider, Flex } from '@chakra-ui/react';
import { AppProps } from 'next/dist/next-server/lib/router/router';
import { QueryClient, QueryClientProvider } from 'react-query';
import { HeaderBar } from '../components/HeaderBar';
import '../styles/globals.css';

const queryClient = new QueryClient();

const MyApp: React.FC<AppProps> = ({ Component, pageProps }) => {
  return (
    <QueryClientProvider client={queryClient}>
      <ChakraProvider>
        <Flex direction="column">
          <HeaderBar />
          <Component {...pageProps} />
        </Flex>
      </ChakraProvider>
    </QueryClientProvider>
  );
};

export default MyApp;
