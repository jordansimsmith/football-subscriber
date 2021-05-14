import { Center, List } from '@chakra-ui/layout';
import { Spinner } from '@chakra-ui/spinner';
import React from 'react';
import { useQuery } from 'react-query';
import { ISubscription } from '../types/types';
import { SubscriptionListItem } from './SubscriptionListItem';

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
        <SubscriptionListItem subscription={s} apiToken={apiToken} key={s.id} />
      ))}
    </List>
  );
};
