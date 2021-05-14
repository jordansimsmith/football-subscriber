import { IconButton } from '@chakra-ui/button';
import { CloseIcon } from '@chakra-ui/icons';
import { Center, HStack, List, ListItem, Text } from '@chakra-ui/layout';
import { Spinner } from '@chakra-ui/spinner';
import React from 'react';
import { useQuery } from 'react-query';
import { ISubscription } from '../types/types';

interface SubscriptionsListProps {
  apiToken: string;
}

export const SubscriptionsList: React.FC<SubscriptionsListProps> = ({
  apiToken,
}) => {
  const { data, isLoading } = useQuery<ISubscription[]>(
    'subscriptions',
    async () => {
      const res = await fetch('http://localhost:5000/subscriptions', {
        headers: {
          Authorization: `Bearer ${apiToken}`,
        },
      });
      const data = await res.json();
      return data;
    },
  );

  if (isLoading) {
    return (
      <Center>
        <Spinner />
      </Center>
    );
  }

  return (
    <List>
      {data?.map((s) => (
        <ListItem key={s.id}>
          <HStack>
            <IconButton
              aria-label="Delete subscription"
              icon={<CloseIcon />}
              colorScheme="red"
              variant="ghost"
            />
            <Text>{s.teamName}</Text>
          </HStack>
        </ListItem>
      ))}
    </List>
  );
};
