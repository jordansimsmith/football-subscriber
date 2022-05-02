import React from 'react';
import { useMutation, useQueryClient } from 'react-query';
import {
  ListItem,
  HStack,
  IconButton,
  Text,
  Alert,
  AlertIcon,
  AlertDescription,
} from '@chakra-ui/react';
import { CloseIcon } from '@chakra-ui/icons';
import { ISubscription } from '../types/types';
import { getAccessToken } from '../lib/api';

interface SubscriptionListItemProps {
  subscription: ISubscription;
}

export const SubscriptionListItem = ({
  subscription,
}: SubscriptionListItemProps): JSX.Element => {
  const queryClient = useQueryClient();

  const { mutate, isLoading, isError } = useMutation(
    async (subscriptionId: number) => {
      const accessToken = await getAccessToken();

      const url = `${process.env.NEXT_PUBLIC_SERVER_BASE}/subscriptions/${subscriptionId}`;
      const res = await fetch(url, {
        method: 'DELETE',
        headers: {
          Authorization: `Bearer ${accessToken}`,
        },
      });
      if (!res.ok) {
        throw new Error(await res.text());
      }
    },
    {
      onSuccess: () => {
        queryClient.invalidateQueries('subscriptions');
      },
    },
  );

  return (
    <ListItem key={subscription.id}>
      {isError && (
        <Alert status="error">
          <AlertIcon />
          <AlertDescription>
            There was an error deleting your subscription
          </AlertDescription>
        </Alert>
      )}

      <HStack>
        <IconButton
          aria-label="Delete subscription"
          icon={<CloseIcon />}
          colorScheme="red"
          variant="ghost"
          onClick={() => mutate(subscription.id)}
          isLoading={isLoading}
        />
        <Text>{subscription.teamName}</Text>
      </HStack>
    </ListItem>
  );
};
