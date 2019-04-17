import numpy as np
from collections import namedtuple
from matplotlib import pyplot as plt

# Order for beating other types:
# Water --> Fire --> Air --> Earth
TYPE_ORDERING = ['water', 'fire', 'air', 'earth']
STARTING_HEALTH = 1.0

ENEMY_ACCURACY = 0.6

# If matchup is favorable, accuracy is boosted by +0.2.
MATCHUP_ACCURACY_BONUS = 0.2

Attack = namedtuple('Attack', 'name accuracy damage')
Punch = Attack('Punch', accuracy=0.8, damage=0.2)
Kick = Attack('Kick', accuracy=0.6, damage=0.4)
Tackle = Attack('Tackle', accuracy=0.4, damage=0.6)

Move = namedtuple('Move', 'type attack success')

ATTRIBUTES = ['scaly', 'furry', 'smooth']
P_ATTR_GIVEN_TYPE = np.array([
  # Scaly   Furry   Smooth
  [0.6,     0.3,    0.1], # Water
  [0.3,     0.1,    0.6], # Fire
  [0.3,     0.4,    0.3], # Air
  [0.1,     0.6,    0.3]  # Earth
])

LOCATIONS = ['ocean', 'volcano', 'underground', 'cloud', 'plains', 'monster']
P_TYPE_GIVEN_LOCATION = np.array([
  # Water   Fire    Air     Earth
  [0.4,     0.1,    0.2,    0.3], # Ocean
  [0.1,     0.4,    0.2,    0.3], # Volcano
  [0.2,     0.3,    0.1,    0.4], # Underground
  [0.3,     0.2,    0.4,    0.1], # Cloud
  [0.25,    0.25,   0.25,   0.25], # Plains
  [0.0,     0.8,    0.1,    0.1] # Monster truck
])

def SampleEmpirical(ptable, nsamples=100):
  """
  Given a true distribution represented as a table, sample an empirical
  distribution from it. Each row in ptable should sum to one.
  """
  empirical = np.zeros_like(ptable)

  rows = ptable.shape[0]
  cols = ptable.shape[1]

  for i in range(rows):
    dist = ptable[i]
    js = np.random.choice(cols, nsamples, p=dist)
    for j in js: empirical[i, j] += 1

  return empirical

def GetMatchupMultiplier(player_type, enemy_type):
  i1 = TYPE_ORDERING.index(player_type.lower())
  i2 = TYPE_ORDERING.index(enemy_type.lower())

  # Player has advantage.
  if i2 == ((i1 + 1) % len(TYPE_ORDERING)):
    return 1
  elif i2 == ((i1 - 1) % len(TYPE_ORDERING)):
    return -1
  else:
    return 0

def ClipZeroOne(v):
  return min(1.0, max(0.0, v))

def ComputePosterior(location, attribute, player_moves=[], enemy_moves=[],
                     p_type_given_location=P_TYPE_GIVEN_LOCATION,
                     p_attr_given_type=P_ATTR_GIVEN_TYPE):
  """
  Compute the probability distribution over enemy types given the player's
  type, location, the visual attributes of the enemy, and the moves done so far
  (and their results).
  """
  probability_log = []

  location_idx = LOCATIONS.index(location.lower())
  attr_idx = ATTRIBUTES.index(attribute.lower())

  # Incorporate location prior and attribute info.
  P_enemy_types = p_type_given_location[location_idx]

  P_enemy_types *= p_attr_given_type[:,attr_idx]
  P_enemy_types /= np.sum(P_enemy_types) # Normalize.

  print('Distribution of enemy types (location + attribute)')
  for i in range(4):
    print('>> %s: %f' % (TYPE_ORDERING[i], P_enemy_types[i]))

  if len(player_moves) > 0:
    for rnd, player_move in enumerate(player_moves):
      # Moves against opponent.
      for i in range(len(TYPE_ORDERING)):
        possible_opponent = TYPE_ORDERING[i]
        matchup_multiplier = GetMatchupMultiplier(player_move.type, possible_opponent)
        accuracy_with_bonus = ClipZeroOne(player_move.attack.accuracy + matchup_multiplier*MATCHUP_ACCURACY_BONUS)

        if player_move.success == True:
          P_enemy_types[i] *= accuracy_with_bonus
        else:
          P_enemy_types[i] *= (1.0 - accuracy_with_bonus)

      # Opponent's moves against you.
      if rnd < len(enemy_moves):
        enemy_move = enemy_moves[rnd]

        for i in range(len(TYPE_ORDERING)):
          possible_opponent = TYPE_ORDERING[i]
          matchup_multiplier = GetMatchupMultiplier(possible_opponent, player_move.type)
          accuracy_with_bonus = ClipZeroOne(ENEMY_ACCURACY + matchup_multiplier*MATCHUP_ACCURACY_BONUS)

          if enemy_move.success == True:
            P_enemy_types[i] *= accuracy_with_bonus
          else:
            P_enemy_types[i] *= (1.0 - accuracy_with_bonus)

    P_enemy_types /= np.sum(P_enemy_types)
    print('Distribution of enemy types (+ moves)')
    for i in range(4):
      print('>> %s: %f' % (TYPE_ORDERING[i], P_enemy_types[i]))

  return P_enemy_types

def Plot(posterior):
  MOVES = [ Move('Air', Punch, False) ]
  ENEMY_MOVES = [ Move('Fire', Punch, True) ]
  x = np.arange(4)
  plt.bar(x, posterior, color=('DarkBlue', 'Red', 'LightBlue', 'Green'))
  plt.xticks(x, ('Water', 'Fire', 'Air', 'Earth'))
  plt.yticks(np.arange(0, 1.1, 0.1))
  plt.title('Distribution of Enemy Types')
  plt.show()

if __name__ == '__main__':
  YOUR_MOVES = [
    Move('Earth', Punch, True),
    Move('Earth', Kick, True)
  ]

  ENEMY_MOVES = [
    Move('UNKNOWN', Kick, False),
    Move('UNKNOWN', Kick, False)
  ]

  p = ComputePosterior(
    'Ocean',
    'Furry',
    player_moves=YOUR_MOVES,
    enemy_moves=ENEMY_MOVES)

  Plot(p)
