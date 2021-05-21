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

interface SubscriptionListItemProps {
  subscription: ISubscription;
  apiToken: string;
}

export const SubscriptionListItem: React.FC<SubscriptionListItemProps> = ({
  subscription,
  apiToken,
}) => {
  const queryClient = useQueryClient();

  const { mutate, isLoading, isError } = useMutation(
    async (subscriptionId: number) => {
      const url = `${process.env.NEXT_PUBLIC_SERVER_BASE}/subscriptions/${subscriptionId}`;
      const res = await fetch(url, {
        method: 'DELETE',
        headers: {
          Authorization: `Bearer ${apiToken}`,
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
