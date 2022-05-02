import { Center, List } from '@chakra-ui/layout';
import { Spinner } from '@chakra-ui/spinner';
import React from 'react';
import { useQuery } from 'react-query';
import { getAccessToken } from '../lib/api';
import { ISubscription } from '../types/types';
import { SubscriptionListItem } from './SubscriptionListItem';

export const SubscriptionsList = (): JSX.Element => {
  const { data, isLoading } = useQuery<ISubscription[]>(
    'subscriptions',
    async () => {
      const accessToken = await getAccessToken();

      const url = `${process.env.NEXT_PUBLIC_SERVER_BASE}/subscriptions`;
      const res = await fetch(url, {
        headers: {
          Authorization: `Bearer ${accessToken}`,
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
        <SubscriptionListItem subscription={s} key={s.id} />
      ))}
    </List>
  );
};
