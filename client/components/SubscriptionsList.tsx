import { Box } from '@chakra-ui/layout';
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

  return (
    <Box>
      {data?.map((s) => (
        <div key={s.id}>{s.teamName}</div>
      ))}
    </Box>
  );
};
