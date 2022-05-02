import { getAccessToken } from '@auth0/nextjs-auth0';
import { NextApiRequest, NextApiResponse } from 'next';

const token = async (
  req: NextApiRequest,
  res: NextApiResponse,
): Promise<void> => {
  try {
    const token = await getAccessToken(req, res);
    res.json(token);
  } catch (error) {
    console.error(error);
    res.status(error.status || 500).end(error.message);
  }
};

export default token;
